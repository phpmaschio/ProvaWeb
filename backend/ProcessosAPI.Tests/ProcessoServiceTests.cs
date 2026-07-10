using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ProcessosAPI;
using ProcessosAPI.Data;
using ProcessosAPI.DTOs;
using ProcessosAPI.Exceptions;
using ProcessosAPI.Models;
using ProcessosAPI.Profiles;
using ProcessosAPI.Services;
using Xunit;

namespace ProcessosAPI.Tests;

public class ProcessoServiceTests
{
    private static (ProcessoService service, ProcessoApiContext context) CriarServico()
    {
        // O provider InMemory não suporta transações reais; ignoramos esse aviso
        // (BeginTransaction vira no-op, mas o comportamento testado não depende disso).
        var options = new DbContextOptionsBuilder<ProcessoApiContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        var context = new ProcessoApiContext(options);

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ProcessoProfile>();
            cfg.AddProfile<ParteProfile>();
            cfg.AddProfile<AndamentoProfile>();
            cfg.AddProfile<StatusProcessoProfile>();
        });
        IMapper mapper = mapperConfig.CreateMapper();

        var statusProcessoService = new StatusProcessoService(context, mapper);
        var parteProcessoService = new ParteProcessoService(context, mapper);
        var parteService = new ParteService(context, mapper);
        var andamentoService = new AndamentoService(context, mapper);

        var service = new ProcessoService(
            statusProcessoService,
            parteProcessoService,
            parteService,
            andamentoService,
            context,
            mapper);

        return (service, context);
    }

    private static StatusProcesso CriarStatus(ProcessoApiContext context, string descricao = "Ativo")
    {
        var status = new StatusProcesso { Descricao = descricao, CriadoEm = DateTime.UtcNow };
        context.StatusProcessos.Add(status);
        context.SaveChanges();
        return status;
    }

    private static Parte CriarParte(ProcessoApiContext context, string nome = "Jake", string tipo = "Interessada")
    {
        var parte = new Parte { Nome = nome, Tipo = tipo, CriadoEm = DateTime.UtcNow };
        context.Partes.Add(parte);
        context.SaveChanges();
        return parte;
    }

    private static CreateProcessoDto CriarProcessoDto(long statusId, List<Parte> partes, string andamentoDescricao = "Petição inicial protocolada")
    {
        return new CreateProcessoDto(
            "Processo de teste",
            statusId,
            partes.Select(p => new ReadParteDto(p.Id, p.Nome, p.Tipo)).ToList(),
            new CreateAndamentoDto(andamentoDescricao)
        );
    }

    [Fact]
    public void CadastrarProcesso_DeveCriarProcessoComPartesEAndamento()
    {
        var (service, context) = CriarServico();
        var status = CriarStatus(context);
        var parte = CriarParte(context);

        var resultado = service.CadastrarProcesso(CriarProcessoDto(status.Id, new List<Parte> { parte }));

        Assert.True(resultado.Id > 0);
        Assert.Equal("Processo de teste", resultado.Descricao);
        Assert.Equal(status.Id, resultado.StatusProcesso.Id);
        Assert.Single(context.Andamentos);
        Assert.Single(context.PartesProcessos);
    }

    [Fact]
    public void CadastrarProcesso_QuandoStatusNaoExiste_LancaNotFoundException()
    {
        var (service, context) = CriarServico();
        var parte = CriarParte(context);

        Assert.Throws<NotFoundException>(() =>
            service.CadastrarProcesso(CriarProcessoDto(statusId: 999, new List<Parte> { parte })));
    }

    [Fact]
    public void CadastrarProcesso_QuandoParteNaoExiste_LancaNotFoundException()
    {
        var (service, context) = CriarServico();
        var status = CriarStatus(context);
        var parteInexistente = new Parte { Id = 999, Nome = "Fantasma", Tipo = "Interessada" };

        Assert.Throws<NotFoundException>(() =>
            service.CadastrarProcesso(CriarProcessoDto(status.Id, new List<Parte> { parteInexistente })));
    }

    [Fact]
    public void BuscarProcessoPorIdDto_DeveRetornarPartesEAndamentoCompletos()
    {
        var (service, context) = CriarServico();
        var status = CriarStatus(context);
        var parte = CriarParte(context);
        var criado = service.CadastrarProcesso(CriarProcessoDto(status.Id, new List<Parte> { parte }));

        var resultado = service.BuscarProcessoPorIdDto(criado.Id);

        Assert.NotEmpty(resultado.Partes);
        Assert.NotNull(resultado.Andamento);
    }

    [Fact]
    public void BuscarProcessoPorIdDto_QuandoNaoExiste_LancaNotFoundException()
    {
        var (service, _) = CriarServico();

        Assert.Throws<NotFoundException>(() => service.BuscarProcessoPorIdDto(999));
    }

    [Fact]
    public void AtualizarProcesso_QuandoDescricaoAndamentoIgual_NaoDuplicaAndamento()
    {
        var (service, context) = CriarServico();
        var status = CriarStatus(context);
        var parte = CriarParte(context);
        var criado = service.CadastrarProcesso(CriarProcessoDto(status.Id, new List<Parte> { parte }, "Aguardando análise"));

        var updateDto = new UpdateProcessoDto(
            "Processo de teste",
            status.Id,
            new List<ReadParteDto> { new ReadParteDto(parte.Id, parte.Nome, parte.Tipo) },
            new CreateAndamentoDto("aguardando análise") // mesma descrição, case diferente
        );
        service.AtualizarProcesso(criado.Id, updateDto);

        var totalAndamentos = context.Andamentos.Count(a => a.Processo.Id == criado.Id);
        Assert.Equal(1, totalAndamentos);
    }

    [Fact]
    public void AtualizarProcesso_QuandoDescricaoAndamentoMuda_CriaNovoAndamento()
    {
        var (service, context) = CriarServico();
        var status = CriarStatus(context);
        var parte = CriarParte(context);
        var criado = service.CadastrarProcesso(CriarProcessoDto(status.Id, new List<Parte> { parte }, "Aguardando análise"));

        var updateDto = new UpdateProcessoDto(
            "Processo de teste",
            status.Id,
            new List<ReadParteDto> { new ReadParteDto(parte.Id, parte.Nome, parte.Tipo) },
            new CreateAndamentoDto("Audiência agendada")
        );
        service.AtualizarProcesso(criado.Id, updateDto);

        var totalAndamentos = context.Andamentos.Count(a => a.Processo.Id == criado.Id);
        Assert.Equal(2, totalAndamentos);

        var atual = service.BuscarProcessoPorIdDto(criado.Id);
        Assert.Equal("Audiência agendada", atual.Andamento!.Descricao);
    }

    [Fact]
    public void AtualizarProcesso_QuandoProcessoNaoExiste_LancaNotFoundException()
    {
        var (service, context) = CriarServico();
        var status = CriarStatus(context);
        var parte = CriarParte(context);

        var updateDto = new UpdateProcessoDto(
            "Processo de teste",
            status.Id,
            new List<ReadParteDto> { new ReadParteDto(parte.Id, parte.Nome, parte.Tipo) },
            new CreateAndamentoDto("Qualquer coisa")
        );

        Assert.Throws<NotFoundException>(() => service.AtualizarProcesso(999, updateDto));
    }

    [Fact]
    public void AtualizarProcesso_DeveAdicionarERemoverPartes()
    {
        var (service, context) = CriarServico();
        var status = CriarStatus(context);
        var parte1 = CriarParte(context, "Jake", "Interessada");
        var parte2 = CriarParte(context, "Fábio", "Contrária");
        var criado = service.CadastrarProcesso(CriarProcessoDto(status.Id, new List<Parte> { parte1 }));

        var updateDto = new UpdateProcessoDto(
            "Processo de teste",
            status.Id,
            new List<ReadParteDto> { new ReadParteDto(parte2.Id, parte2.Nome, parte2.Tipo) },
            new CreateAndamentoDto("Petição inicial protocolada")
        );
        service.AtualizarProcesso(criado.Id, updateDto);

        var vinculos = context.PartesProcessos.Include(pp => pp.Parte).Where(pp => pp.Processo.Id == criado.Id).ToList();
        Assert.Single(vinculos);
        Assert.Equal(parte2.Id, vinculos[0].Parte.Id);
    }

    [Fact]
    public void DeletarProcesso_DeveRemoverProcesso()
    {
        var (service, context) = CriarServico();
        var status = CriarStatus(context);
        var parte = CriarParte(context);
        var criado = service.CadastrarProcesso(CriarProcessoDto(status.Id, new List<Parte> { parte }));

        service.DeletarProcesso(criado.Id);

        Assert.Throws<NotFoundException>(() => service.BuscarProcessoPorIdDto(criado.Id));
    }

    [Fact]
    public void DeletarProcesso_QuandoNaoExiste_LancaNotFoundException()
    {
        var (service, _) = CriarServico();

        Assert.Throws<NotFoundException>(() => service.DeletarProcesso(999));
    }

    [Fact]
    public void ListarProcessos_DeveClamparSkipETakeInvalidos()
    {
        var (service, context) = CriarServico();
        var status = CriarStatus(context);
        var parte = CriarParte(context);
        service.CadastrarProcesso(CriarProcessoDto(status.Id, new List<Parte> { parte }));
        service.CadastrarProcesso(CriarProcessoDto(status.Id, new List<Parte> { parte }));

        var resultado = service.ListarProcessos(skip: -10, take: 500);

        Assert.Equal(2, resultado.Count);
    }
}
