import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { catchError, throwError } from 'rxjs';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const snackBar = inject(MatSnackBar);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      const mensagem = error.error?.message ?? 'Ocorreu um erro inesperado. Tente novamente.';
      snackBar.open(mensagem, 'Fechar', { duration: 5000 });
      console.error(error);
      return throwError(() => error);
    })
  );
};
