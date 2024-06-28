using ClassMate.Domain.Abstractions.Repositories;
using ClassMate.Domain.Abstractions.Services;
using ClassMate.Domain.Dtos;
using ClassMate.Domain.Enums;
using ClassMate.Domain.Exceptions;
using ClassMate.Domain.Settings;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Requests;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Tasks.v1;
using Google.Apis.Util.Store;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassMate.Services
{
    public class GoogleService : IGoogleService
    {
        private readonly GoogleSettings _googleSettings;
        private readonly IOAuthTokenRepository _tokenRepository;
        private readonly IUserContextService _userContextService;

        public GoogleService(IOptions<GoogleSettings> googleSettings, IOAuthTokenRepository oAuthTokenRepository, IUserContextService userContextService)
        {
            _googleSettings = googleSettings.Value;
            _tokenRepository = oAuthTokenRepository;
            _userContextService = userContextService;
        }

        public string GetAuthUrl()
        {
            var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets
                {
                    ClientId = _googleSettings.ClientID,
                    ClientSecret = _googleSettings.ClientSecret
                },
                Scopes = new[] { CalendarService.Scope.Calendar, TasksService.Scope.Tasks }
            });

            var redirectUri =   _googleSettings.RedirectUri;
            var authUri = flow.CreateAuthorizationCodeRequest(redirectUri).Build();
            return authUri.AbsoluteUri;
        }

        public async Task<string> Callback(string code)
        {
            try
            {
                var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
                {
                    ClientSecrets = new ClientSecrets
                    {
                        ClientId = _googleSettings.ClientID,
                        ClientSecret = _googleSettings.ClientSecret
                    },
                    Scopes = new[] { CalendarService.Scope.Calendar }
                });

                var redirectUri = _googleSettings.RedirectUri;
                var token = await flow.ExchangeCodeForTokenAsync("user", code, redirectUri, CancellationToken.None);

                var OAuthToken = new OAuthTokenDto
                {
                    AccessToken = token.AccessToken,
                    RefreshToken = token.RefreshToken,
                    IssuedUtc = token.IssuedUtc,
                    ExpiresInSeconds = token.ExpiresInSeconds
                };

                await SaveTokenAsync(OAuthToken);

                return "Authentication successful! You can now access Google Calendar API.";
            }
            catch (Exception ex)
            {
                throw new AppException("Authentication failed: " + ex.Message);
            }
        }


        public async Task<CalendarEventDto?> SaveClass(ClassDto classDto, string? eventId)
        {
            var token = await GetToken();
            
            var calendarService = new CalendarService(new BaseClientService.Initializer
            {
                HttpClientInitializer = new UserCredential(new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
                {
                    ClientSecrets = new ClientSecrets
                    {
                        ClientId = _googleSettings.ClientID,
                        ClientSecret = _googleSettings.ClientSecret
                    }
                }), "user", new TokenResponse { AccessToken = token.AccessToken, RefreshToken = token.RefreshToken, IssuedUtc = token.IssuedUtc, ExpiresInSeconds = token.ExpiresInSeconds }),
                ApplicationName = "Class Mate"
            });

            var googleEvent = new Event
            {
                Summary = classDto.Title,
                Description = classDto.Description,
                Start = new EventDateTime { DateTimeDateTimeOffset = classDto.StartDate, TimeZone = "UTC" },
                End = new EventDateTime { DateTimeDateTimeOffset = classDto.EndDate, TimeZone = "UTC"}
            };

            if (classDto.RepeatUntil != null)
            {
                var rrule = $"RRULE:FREQ=WEEKLY;INTERVAL={classDto.WeekRepetition};" +
                    $"BYDAY={classDto.StartDate.DayOfWeek.ToString().Substring(0, 2).ToUpperInvariant()};" +
                    $"UNTIL={classDto.RepeatUntil.Value.ToString("yyyyMMdd")}T235959Z";

                googleEvent.Recurrence = [rrule];
            }

            if (eventId != null)
            {
                var request = calendarService.Events.Update(googleEvent, "primary", eventId);
                await request.ExecuteAsync();
                return null;
            }
            else
            {
                var request = calendarService.Events.Insert(googleEvent, "primary");
                var gEvent = await request.ExecuteAsync();

                return new CalendarEventDto()
                {
                    Platform = Platforms.Google,
                    PlatformEventId = gEvent.ICalUID
                };
            }
        }

        public async Task<CalendarEventDto?> SaveToDo(ToDoDto toDoDto, string? eventId)
        {
            var token = await GetToken();

            var tasksService = new TasksService(new BaseClientService.Initializer
            {
                HttpClientInitializer = new UserCredential(new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
                {
                    ClientSecrets = new ClientSecrets
                    {
                        ClientId = _googleSettings.ClientID,
                        ClientSecret = _googleSettings.ClientSecret
                    }
                }), "user", new TokenResponse { AccessToken = token.AccessToken, RefreshToken = token.RefreshToken, IssuedUtc = token.IssuedUtc, ExpiresInSeconds = token.ExpiresInSeconds }),
                ApplicationName = "Class Mate"
            });

            var googleTask = new Google.Apis.Tasks.v1.Data.Task
            {
                Title = toDoDto.Title,
                Notes = toDoDto.Description,
                Due = toDoDto.Deadline.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                Status = toDoDto.Done ? "completed" : "needsAction"
            };
            

            if (eventId != null)
            {
                var request = tasksService.Tasks.Update(googleTask, "@default", eventId);
                await request.ExecuteAsync();
                return null;
            }
            else
            {
                var request = tasksService.Tasks.Insert(googleTask, "@default");
                var gTask = await request.ExecuteAsync();

                return new CalendarEventDto()
                {
                    Platform = Platforms.Google,
                    PlatformEventId = gTask.Id
                };
            }
        }

        private async Task SaveTokenAsync(OAuthTokenDto token)  
        {

            var userId = _userContextService.GetCurrentUserID();

            var existingToken = await _tokenRepository.GetByUserIdAndPlatformAsync(userId, Platforms.Google);

            if (existingToken != null)
            {
                existingToken.AccessToken = token.AccessToken;
                existingToken.RefreshToken = token.RefreshToken;

                existingToken.IssuedUtc = token.IssuedUtc;
                existingToken.ExpiresInSeconds = token.ExpiresInSeconds;

                await _tokenRepository.UpdateAsync(existingToken);
            }
            else
            {
                await _tokenRepository.CreateAsync(new OAuthTokenDto()
                {
                    AccessToken = token.AccessToken,
                    RefreshToken = token.RefreshToken,
                    IssuedUtc = token.IssuedUtc,
                    ExpiresInSeconds = token.ExpiresInSeconds, 
                    UserId = userId,
                    Platform = Platforms.Google
                });
            }
        }

        private async Task<OAuthTokenDto> GetToken()
        {
            var userId = _userContextService.GetCurrentUserID();
            var existingToken = await _tokenRepository.GetByUserIdAndPlatformAsync(userId, Platforms.Google)
                                ?? throw new NotFoundException("No existing token found.");

            var expirationTime = existingToken.IssuedUtc.AddSeconds(existingToken.ExpiresInSeconds ?? 0);
            var remainingTime = expirationTime - DateTime.UtcNow;

            if (remainingTime.TotalMinutes < 2)
            {
                existingToken = await RefreshToken(existingToken);
            }

            return existingToken;
        }

        private async Task<OAuthTokenDto> RefreshToken(OAuthTokenDto existingToken)
        {
            var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets
                {
                    ClientId = _googleSettings.ClientID,
                    ClientSecret = _googleSettings.ClientSecret
                }
            });
            try
            {
                var tokenResponse = await flow.RefreshTokenAsync("user", existingToken.RefreshToken, CancellationToken.None);

                existingToken.AccessToken = tokenResponse.AccessToken;
                existingToken.IssuedUtc = DateTime.UtcNow;
                existingToken.ExpiresInSeconds = tokenResponse.ExpiresInSeconds;

                await _tokenRepository.UpdateAsync(existingToken);

                return existingToken;
            }
            catch (Exception)
            {
                throw new NotFoundException("No existing token found.");
            }

        }


    }
}
