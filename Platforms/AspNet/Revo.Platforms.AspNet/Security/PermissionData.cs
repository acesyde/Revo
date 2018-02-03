﻿using System;

namespace Revo.Platforms.AspNet.Security
{
    public struct PermissionData
    {
        public Guid PermissionTypeId { get; set; }
        public Guid? ResourceId { get; set; }
        public Guid? ContextId { get; set; }
    }
}