export function getCurrentWeekWindow() {
   const months = [
      "Jan",
      "Feb",
      "Mar",
      "Apr",
      "May",
      "Jun",
      "Jul",
      "Aug",
      "Sep",
      "Oct",
      "Nov",
      "Dec",
   ];

   const today = new Date();
   //find date of monday
   const monday = new Date(today);
   const dayOfWeek = today.getDay();
   const daysToMonday = dayOfWeek === 0 ? 6 : dayOfWeek - 1; // Adjust for Sunday
   monday.setDate(today.getDate() - daysToMonday);
   //find date of saturday
   const saturday = new Date(monday);
   saturday.setDate(monday.getDate() + 5);
   const formatDate = (date) =>
      `${months[date.getMonth()]} ${date.getDate()}, ${date.getFullYear()}`;

   return `${formatDate(monday)} - ${formatDate(saturday)}`;
}
