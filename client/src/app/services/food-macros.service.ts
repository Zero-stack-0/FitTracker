import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { FoodMacrosInterface } from '../models/food-macros-interface';
import { API_BASE_URL } from '../Constants/api-urls';

@Injectable({
  providedIn: 'root'
})
export class FoodMacrosService {

  constructor(private http: HttpClient) { }

  getFoodByName(name: string): Observable<FoodMacrosInterface[]> {
    return this.http.get<{ data: FoodMacrosInterface[] }>(API_BASE_URL + `/api/FoodMacro?name=${name}`)
      .pipe(map(res => res.data));
  }
}
