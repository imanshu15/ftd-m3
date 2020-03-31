import { NgModule, ModuleWithProviders } from '@angular/core';
import { FtdM3ClientComponent } from './ftd-m3-client.component';
import { FtdM3ClientService } from './ftd-m3-client.service';
import { M3Config } from './models/export.model';
import { CommonModule } from '@angular/common';
import { HttpService } from './helpers/http.service';
import { HttpClientModule } from '@angular/common/http';



@NgModule({
  declarations: [FtdM3ClientComponent],
  imports: [
    CommonModule,
    HttpClientModule
  ],
  exports: [FtdM3ClientComponent],
  providers: [ HttpService, FtdM3ClientService ]
})
export class FtdM3ClientModule {

  static forRoot(config: M3Config): ModuleWithProviders {
    return {
      ngModule: FtdM3ClientModule,
      providers: [
        FtdM3ClientService, {provide: 'config', useValue: config }
      ]
    };
  }

 }
