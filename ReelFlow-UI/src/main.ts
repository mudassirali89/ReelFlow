import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/components/root/app.config';
import { App } from './app/components/root/app';

bootstrapApplication(App, appConfig)
  .catch((err) => console.error(err));
