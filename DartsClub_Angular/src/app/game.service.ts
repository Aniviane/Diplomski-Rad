import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { GameDTO } from './models/GameDTO';
import { Observable } from 'rxjs';
import { AveragesDTO } from './models/AveragesDTO';
import { FullGameDTO } from './models/FullGameDTO';

@Injectable({
  providedIn: 'root'
})
export class GameService {
  BaseUrl =  "https://localhost:44314/"


  constructor(private http:HttpClient) { }

  postGame(game : FullGameDTO) : Observable<boolean> {
    return this.http.post<boolean>(this.BaseUrl + 'api/Game/', game);
  }

  getGames(id : string) : Observable<GameDTO[]> {
    return this.http.get<GameDTO[]>(this.BaseUrl + 'api/Game/PlayerId/' + id) 
  }

  getAverages(id : string) : Observable<AveragesDTO> {
    return this.http.get<AveragesDTO>(this.BaseUrl + 'api/Game/Averages/' + id) 
  }

}
