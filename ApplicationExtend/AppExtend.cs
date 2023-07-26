 using Microsoft.AspNetCore.Builder;
 using System.Net;
 using Microsoft.AspNetCore.Http;
 
 namespace webmvc.ApplicationExtend
 {
    public static class AppExtend
    {
        public static void addStatusCodepages(this IApplicationBuilder app ){
            app.UseStatusCodePages(appEroor =>{
                appEroor.Run(async context =>{
                    var respone = context.Response;
                    var code = respone.StatusCode;
                    
                    var content = @$"<html> 
                            <head>
                            <meta charset='UTF8' />
                                <title> LOi {code} </title>
                            </head>
                            <body>s
                                <p style='color:red; font-size:30px'> 
                                co loi xay ra : {code}-{(HttpStatusCode)code}
                                </p>
                            </body>

                    </html>";
                   await respone.WriteAsync(content);
                } );
            });
        }
    }
 }