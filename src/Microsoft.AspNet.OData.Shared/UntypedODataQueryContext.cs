// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Microsoft.AspNet.OData.Common;
using Microsoft.AspNet.OData.Formatter;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNet.OData.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OData.Edm;

namespace Microsoft.AspNet.OData
{
    /// <summary>
    /// This defines some context information used to perform query composition.
    /// </summary>
    public class UntypedODataQueryContext
    {
        private DefaultQuerySettings _defaultQuerySettings;

        /// <summary>
        /// Constructs an instance of <see cref="ODataQueryContext"/> with <see cref="IEdmModel" />, element CLR type,
        /// and <see cref="ODataPath" />.
        /// </summary>
        /// <param name="path">The parsed <see cref="ODataPath"/>.</param>
        /// <remarks>
        /// This is a public constructor used for stand-alone scenario; in this case, the services
        /// container may not be present.
        /// </remarks>
        public UntypedODataQueryContext(ODataPath path)
        {
            Path = path;
            GetPathContext();
        }
   
        /// <summary>
        /// Gets the given <see cref="DefaultQuerySettings"/>.
        /// </summary>
        public DefaultQuerySettings DefaultQuerySettings
        {
            get
            {
                if (_defaultQuerySettings == null)
                {
                    _defaultQuerySettings = RequestContainer == null
                        ? new DefaultQuerySettings()
                        : RequestContainer.GetRequiredService<DefaultQuerySettings>();
                }

                return _defaultQuerySettings;
            }
        }

        /// <summary>
        /// Gets the <see cref="ODataPath"/>.
        /// </summary>
        public ODataPath Path { get; private set; }

        /// <summary>
        /// Gets the request container.
        /// </summary>
        /// <remarks>
        /// The services container may not be present. See the constructor in this file for
        /// use in stand-alone scenarios.
        /// </remarks>
        public IServiceProvider RequestContainer { get; internal set; }

        internal string TargetName { get; private set; }

        private void GetPathContext()
        {
            if (Path != null)
            {
                IEdmProperty property;
                IEdmStructuredType structuredType;
                string name;
                EdmLibHelpers.GetPropertyAndStructuredTypeFromPath(
                    Path.Segments,
                    out property,
                    out structuredType,
                    out name);

                TargetName = name;
            }
        }
    }
}
