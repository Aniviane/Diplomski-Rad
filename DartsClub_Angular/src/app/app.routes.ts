import { Routes } from '@angular/router';
import { UserComponent } from './user/user.component';
import { BlogComponent } from './blog/blog.component';
import { GameComponent } from './game/game.component';

export const routes: Routes = [
    {path : "Users", component: UserComponent},
    {path : "Blogs", component: BlogComponent},
    {path : "Games", component: GameComponent}
];
