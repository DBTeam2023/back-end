﻿using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UserIdentification.application;
using UserIdentification.domain.model;
using UserIdentification.domain.service;
using UserIdentification.dto;
using UserIdentification.exception;
using UserIdentification.resource.vo;
using UserIdentification.utils;

/**
 * @author sty
 * @implnote use "[action]" in URL for allowing asynchronous http request
 */
namespace UserIdentification.resource.controller
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserIdentificationController : ControllerBase
    {
        public static JwtHelper jwt;
        public static UserIdentificationService userIdentificationService;

        public UserIdentificationController(UserIdentificationService _userIdentificationService, JwtHelper _jwt)
        {
            jwt = _jwt;
            userIdentificationService = _userIdentificationService;
        }

        [HttpPost]
        public ComResponse<TokenDto> login([FromBody] LoginDto user)
        {
            return ComResponse<TokenDto>.success(userIdentificationService.login(user.Username, user.Password));
        }

        [HttpPost]
        public ComResponse<TokenDto> register([FromBody] RegisterDto user)
        {
            return ComResponse<TokenDto>.success(userIdentificationService.register(user.Username,user.Password,user.Type));
        }

        [HttpGet]
        public async Task<ComResponse<UserAggregate>> getUserInfo()
        {
            //resolve token
            string token = Request.Headers["Authorization"].ToString();
            if(token == null)
            {
                throw new ParamMissingException("token required");
            }
            var newToken = token.Replace("Bearer ", "");
            var username = jwt.resolveToken(newToken);

            return ComResponse<UserAggregate>.success(userIdentificationService.getUserInfoByUsername(username));
        }
    }
}
