import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { UserService } from './user.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'DartsClub_Angular';

  constructor(private service:UserService) {}
  ngOnInit() : void {
   
  }
}
