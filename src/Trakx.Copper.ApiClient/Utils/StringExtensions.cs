﻿using System;

namespace Trakx.Copper.ApiClient.Utils
{
    public static class StringExtensions
    {
        public static string ToHexString(this byte[] array)
        {
            return BitConverter.ToString(array).Replace("-", "").ToLower();
        }
    }
}
