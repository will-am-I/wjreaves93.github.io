$(document).ready(function()
{
   $('#header').ready(function()
   {
      $('#body').ready(function()
      {
         $('#about').click(function()
         {
            $('#body').empty();
            $('#body').load('pages/about.html');
         });
         
         $('#portfolio').click(function()
         {
            $('#body').empty();
            $('#body').load('pages/portfolio.html');
         });
         
         $('#resume').click(function()
         {
            $('#body').empty();
            $('#body').load('pages/resume.html');
         });
         
         $('#clients').click(function()
         {
            $('#body').empty();
            $('#body').load('pages/clients.html');
         });
      });
   });
});