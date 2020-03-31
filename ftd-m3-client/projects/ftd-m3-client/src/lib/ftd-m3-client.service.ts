import { Injectable } from '@angular/core';
import { HttpService } from './helpers/http.service';
import { M3Request } from './models/export.model';

@Injectable({
  providedIn: 'root'
})
export class FtdM3ClientService {

  constructor(private httpService: HttpService) {
   }

   // tslint:disable-next-line: max-line-length
   executeRequest(program: string, transaction: string, param: any = null, output: string[] = [], outputAll: boolean = false, filter: any = null, sort: string[] = [], orderByDesc: boolean = false): Promise<any> {
     const req: M3Request = { program,  transaction,  param,  output, outputAll,  filter, sort, orderByDesc };
     return this.executeRequestV2(req);
   }

   executeRequestV2(request: M3Request): Promise<any> {
     return this.httpService.post('M3', request).toPromise();
  }
}
