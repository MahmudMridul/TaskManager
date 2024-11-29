import {
   Sidebar,
   SidebarContent,
   SidebarGroup,
   SidebarGroupContent,
   SidebarGroupLabel,
   SidebarMenu,
   SidebarMenuButton,
   SidebarMenuItem,
   SidebarProvider,
   SidebarTrigger,
} from "@/components/ui/sidebar";
import { Calendar, ListChecks, Inbox, Search, Settings } from "lucide-react";
import { NavLink, Outlet } from "react-router";

const items = [
   {
      title: "Tasks",
      url: "/home",
      icon: ListChecks,
   },
   {
      title: "Inbox",
      url: "/signin",
      icon: Inbox,
   },
   {
      title: "Calendar",
      url: "#",
      icon: Calendar,
   },
   {
      title: "Search",
      url: "#",
      icon: Search,
   },
   {
      title: "Settings",
      url: "#",
      icon: Settings,
   },
];

export default function Layout() {
   return (
      <SidebarProvider>
         <Sidebar>
            <SidebarContent>
               <SidebarGroup>
                  <SidebarGroupLabel>Task Manager</SidebarGroupLabel>
                  <SidebarGroupContent>
                     <SidebarMenu>
                        {items.map((item) => (
                           <SidebarMenuItem key={item.title}>
                              <SidebarMenuButton asChild>
                                 <NavLink to={item.url}>
                                    <item.icon />
                                    <span>{item.title}</span>
                                 </NavLink>
                              </SidebarMenuButton>
                           </SidebarMenuItem>
                        ))}
                     </SidebarMenu>
                  </SidebarGroupContent>
               </SidebarGroup>
            </SidebarContent>
         </Sidebar>
         <SidebarTrigger />
         <Outlet />
      </SidebarProvider>
   );
}
