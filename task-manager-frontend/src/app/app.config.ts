import { ApplicationConfig, provideBrowserGlobalErrorListeners, importProvidersFrom } from '@angular/core';
import { provideRouter } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { routes } from './app.routes';

/**
 * Configuración global de la aplicación cuando se usa `bootstrapApplication`:
 * - Registra proveedores globales como HttpClientModule y NgbModule.
 * - Configura el router con las rutas definidas en `app.routes`.
 */
export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    importProvidersFrom(HttpClientModule, NgbModule),
    provideRouter(routes)
  ]
};
