import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";

import { HomeComponent } from "./components/home/home.component";
import { LoginComponent } from "./components/login/login.component";
import { SignUpComponent } from "./components/sign-up/sign-up.component";

const routes: Routes = [
  {
    path: "",
    pathMatch: "full",
    redirectTo: "home"
  },
  {
    path: "home",
    component: HomeComponent
  },
  {
    path: "login",
    component: LoginComponent
  },
  {
    path: "sign-up",
    component: SignUpComponent
  },
  {
    path: "**",
    pathMatch: "full",
    redirectTo: "home"
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }