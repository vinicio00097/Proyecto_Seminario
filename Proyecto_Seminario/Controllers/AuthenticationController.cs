﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Proyecto_Seminario.Models;
using Google.Apis.Auth;
using Google.Apis.Auth.OAuth2;

namespace Proyecto_Seminario.Controllers
{
    public class AuthenticationController : Controller
    {
        private GoogleCredentials googleCredentials { get; set; }
        private ModelContext db = new ModelContext();

        public AuthenticationController(IOptions<GoogleCredentials> settings)
        {
            googleCredentials = settings.Value;
        }

        // GET: Authentication
        public async Task<ActionResult> Index()
        {
            if (Request.Cookies["session_token"] != null)
            {
                try
                {
                    GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(Request.Cookies["session_token"]);

                    if(payload.ExpirationTimeSeconds> DateTimeOffset.Now.ToUnixTimeSeconds())
                    {
                        Usuarios usuario=db.Usuarios.Where(item => item.UsuarioEmail == payload.Email).FirstOrDefault();

                        if (usuario != null)
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            return View();
                        }
                    }else
                    {
                        return View();
                    }
                }
                catch(InvalidJwtException exc)
                {
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

        public void Google_Oauth_Signin()
        {  
            Response.Redirect($"https://accounts.google.com/o/oauth2/v2/auth?client_id={googleCredentials.Google_ClientID}&response_type=code&scope=openid%20email%20profile&redirect_uri={googleCredentials.Google_RedirectURL}&state=abcdef");
        }

        [HttpGet]
        public async Task<ActionResult> GetUserInfo(string code, string state, string session_state)
        {
            if (string.IsNullOrEmpty(code))
            {
                return View("Error");
            }

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://www.googleapis.com")
            };
            var requestUrl = $"oauth2/v4/token?code={code}&client_id={googleCredentials.Google_ClientID}&client_secret={googleCredentials.Google_SecretKey}&redirect_uri={googleCredentials.Google_RedirectURL}&grant_type=authorization_code";

            var dict = new Dictionary<string, string>
            {
                { "Content-Type", "application/x-www-form-urlencoded" }
            };
            var req = new HttpRequestMessage(HttpMethod.Post, requestUrl) { Content = new FormUrlEncodedContent(dict) };
            var response = await httpClient.SendAsync(req);
            Debug.WriteLine(await response.Content.ReadAsStringAsync());
            var token = JsonConvert.DeserializeObject<GmailToken>(await response.Content.ReadAsStringAsync());
            Response.Cookies.Append("session_token", token.IdToken);
            await GetuserProfile(token.AccessToken);

            GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(token.IdToken);

            Usuarios usuario = db.Usuarios.Where(item => item.UsuarioEmail == payload.Email).FirstOrDefault();

            if (usuario != null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task GetuserProfile(string accesstoken)
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://www.googleapis.com")
            };
            string url = $"https://www.googleapis.com/oauth2/v1/userinfo?alt=json&access_token={accesstoken}";
            var response = await httpClient.GetAsync(url);
            Debug.WriteLine(response.Content.ReadAsStringAsync());
        }
    }
}