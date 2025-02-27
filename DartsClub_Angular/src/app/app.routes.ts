import { Routes } from '@angular/router';
import { UserComponent } from './user/user.component';
import { BlogComponent } from './blog/blog.component';
import { GameComponent } from './game/game.component';
import { LogRegComponent } from './log-reg/log-reg.component';
import { ReservationComponent } from './reservation/reservation.component';
import { CreateBlogComponent } from './create-blog/create-blog.component';
import { PostGameComponent } from './post-game/post-game.component';
import { HomeComponent } from './home/home.component';

export const routes: Routes = [
    {path : "", component: HomeComponent},
    {path : "User", component: UserComponent},
    {path : "Blogs", component: BlogComponent},
    {path : "Games", component: GameComponent},
    {path : "Login", component: LogRegComponent},
    {path : "Reservation", component: ReservationComponent},
    {path : "AddGame", component: PostGameComponent},
    {path : "Post", component: CreateBlogComponent}
];
