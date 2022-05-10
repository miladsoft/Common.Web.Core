﻿using System;

namespace Common.Web.Core
{
    /// <summary>
    /// Use this attribute to mark the string properties of your Models which should be
    /// encrypted automatically using the `EncryptedFieldResultFilter` and
    /// decrypted using the `EncryptedFieldModelBinderProvider`.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class EncryptedFieldAttribute : Attribute { }
}
