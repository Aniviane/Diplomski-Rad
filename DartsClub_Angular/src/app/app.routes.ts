import { Routes } from '@angular/router';
import { UserComponent } from './user/user.component';
import { BlogComponent } from './blog/blog.component';
import { GameComponent } from './game/game.component';
import { LogRegComponent } from './log-reg/log-reg.component';
import { ReservationComponent } from './reservation/reservation.component';

export const routes: Routes = [
    {path : "User", component: UserComponent},
    {path : "Blogs", component: BlogComponent},
    {path : "Games", component: GameComponent},
    {path : "Login", component: LogRegComponent},
    {path : "Reservation", component: ReservationComponent}
];
