﻿using System.Text.Json.Serialization;

namespace Elefess.Core.Models;

public sealed record LfsRequestObject(
    [property: JsonPropertyName("oid")]
        string Oid, 
    [property: JsonPropertyName("size")]
        long Size);