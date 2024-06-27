import { Component } from '@angular/core';
import { GameDTO } from '../models/GameDTO';
import { CommonModule } from '@angular/common';
import {MatDatepickerInputEvent, MatDatepickerModule} from '@angular/material/datepicker';
import {MatInputModule} from '@angular/material/input';
import {MatFormFieldModule} from '@angular/material/form-field';
import {provideNativeDateAdapter} from '@angular/material/core';
import {MatSelectModule, MatSelectChange} from '@angular/material/select';
import { FormsModule } from '@angular/forms';

import {MatCardModule} from '@angular/material/card';

import {MatButtonModule} from '@angular/material/button';
import { UserService } from '../user.service';
import { GameService } from '../game.service';
import { UsernamesDTO } from '../models/UsernamesDTO';
import { FullGameDTO } from '../models/FullGameDTO';
@Component({
  selector: 'app-post-game',
  standalone: true,
  providers : [provideNativeDateAdapter()],
  imports: [FormsModule,MatCardModule,CommonModule,MatDatepickerModule,MatInputModule,MatSelectModule,MatFormFieldModule,MatButtonModule],
  templateUrl: './post-game.component.html',
  styleUrl: './post-game.component.css'
})
export class PostGameComponent {

  constructor(private userService:UserService, private gameService:GameService) {}


  ngOnInit() {
    this.userService.getUsernames().subscribe(ret => {
      console.log(ret)
      this.usernames = [...ret, {id : "a0a0aa0d-a00a-0000-0a0a-00aa0a000a00", name : "Guest"}]
    })
  }

  personalGames : GameDTO[] = []
  numberOfPlayers : number = 0
  gameType : string = ""
  numberOfRounds : number = 0
  gameDate : Date = new Date

  usernames : UsernamesDTO[] = []

  

  addEvent( event: MatDatepickerInputEvent<Date>) {
    if(!event.value) return
    console.log(event.value)
    
    this.gameDate = event.value

    this.personalGames.forEach(elem => elem.gameDate = event.value || new Date)
  }

  setPlayerCount(event : MatSelectChange) {
    console.log(event.value)


    switch(event.value) {
      case ("1v1") : {
        this.numberOfPlayers = 2
        this.gameType = "1v1"
        break
      }
      case ("FFA3") : {
        this.numberOfPlayers = 3
        this.gameType = "FFA"
        break
      }
      case ("FFA4") : {
        this.numberOfPlayers = 4
        this.gameType = "FFA"
        break
      }
      case ("2v2") : {
        this.numberOfPlayers = 4
        this.gameType = "2v2"
        break
      }
    }

    this.personalGames = []

    for (let i = 0; i < this.numberOfPlayers; i++) {
      let game = new GameDTO
      game.gameDate = this.gameDate
      game.gameType = this.gameType
      game.rounds = this.numberOfRounds
      this.personalGames.push(game)
    }

    console.log(this.personalGames)
  }



  submitGame() {
    let fullGame = new FullGameDTO
    fullGame.date = this.gameDate
    fullGame.gameType = this.gameType
    fullGame.numOfRounds = this.numberOfRounds

    this.personalGames.forEach(elem => {
      fullGame.bullsEyes.push(elem.bullsEyes)
      fullGame.tripleTwentys.push(elem.tripleTwenties)
      fullGame.playerIds.push(elem.userId)
      fullGame.gameScores.push(elem.score)
    })

    console.log(fullGame)
    this.gameService.postGame(fullGame).subscribe(ret =>
      console.log(ret)
    )

  }

  displayGames() {
    console.log(this.personalGames)
  }
}
