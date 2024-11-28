import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { NavLink } from "react-router";

export default function Signup() {
   return (
      <div className="container mx-auto h-screen p-5 flex justify-center items-center">
         <div className="w-1/5">
            <Input type="text" placeholder="Username" />
            <Input type="email" placeholder="Email" />
            <Input type="password" placeholder="Password" />
            <div className="flex justify-between">
               <Button variant="link">
                  <NavLink to="/">Sign in</NavLink>
               </Button>
               <Button>Sign up</Button>
            </div>
         </div>
      </div>
   );
}
