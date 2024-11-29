import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { getCurrentWeekWindow } from "@/helpers/functions";
import { Paperclip, Target, CircleHelp, MessageSquare } from "lucide-react";

const tasks = [
   {
      task: "backend program",
      goal: "2 hours",
      status: "done",
      comment: "test comment",
   },
   {
      task: "system design | needcode sd for begin",
      goal: "3 hours",
      status: "done",
      comment: "next video 13",
   },
   {
      task: "backend program",
      goal: "2 hours",
      status: "done",
      comment: "test comment",
   },
   {
      task: "backend program",
      goal: "2 hours",
      status: "done",
      comment: "test comment",
   },
];

export default function Home() {
   return (
      <div className="container mx-auto p-3">
         <div className="text-xl font-bold">{getCurrentWeekWindow()}</div>
         <div className="my-5">
            {tasks.map((item, index) => {
               return (
                  <Card className="mb-5" key={index}>
                     <CardHeader>
                        <CardTitle>
                           <div className="flex gap-x-2">
                              <Paperclip /> {item.task}
                           </div>
                        </CardTitle>
                     </CardHeader>
                     <CardContent>
                        <div className="flex gap-x-2">
                           <Target /> Goal {item.goal}
                        </div>
                        <div className="flex gap-x-2">
                           <CircleHelp /> Status{item.status}
                        </div>
                        <div className="flex gap-x-2">
                           <MessageSquare /> Comment{item.comment}
                        </div>
                     </CardContent>
                  </Card>
               );
            })}
         </div>
      </div>
   );
}
