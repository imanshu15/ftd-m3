import { Component } from '@angular/core';
import { FtdM3ClientService, M3Request } from 'projects/ftd-m3-client/src/public-api';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'ftd-m3-client';

  constructor(private m3Service: FtdM3ClientService) {
    this.m3Service.executeRequest('ARZ100MI', 'LstFAMFnc', { CONO: '100', DIVI: '130'}, [], true, {}, ['FFNC'], true).then(a => {
      console.log(a);
    });

    this.m3Service.executeRequestV2( { program: 'ARZ100MI', transaction: 'LstFAMFnc', param: { CONO: '100', DIVI: '150'}} ).then(a => {
      console.log(a);
    });
  }
}
