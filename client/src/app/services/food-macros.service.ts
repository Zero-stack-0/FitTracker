import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { FoodMacrosInterface } from '../models/food-macros-interface';

@Injectable({
  providedIn: 'root'
})
export class FoodMacrosService {

  constructor(private http: HttpClient) { }
  private API_BASE_URL = 'http://localhost:5074';

  getFoodByName(name: string): Observable<FoodMacrosInterface[]> {
    return this.http.get<{ data: FoodMacrosInterface[] }>(this.API_BASE_URL + `/api/FoodMacro?name=${name}`)
      .pipe(map(res => res.data));
  }
}
