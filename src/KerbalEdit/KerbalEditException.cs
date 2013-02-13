// -----------------------------------------------------------------------
// <copyright file="KerbalEditException.cs" company="OpenSauceSolutions">
// © 2013 OpenSauce Solutions
// </copyright>
// -----------------------------------------------------------------------

namespace KerbalEdit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;

    /// <summary>
    /// Base Exception used any time an error with the underlying cache API is surfaced. 
    /// </summary>
    public class KerbalEditException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KerbalEditException" /> class.
        /// </summary>
        public KerbalEditException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KerbalEditException" /> class.
        /// </summary>
        /// <param name="message">error message to surface, BE DESCRIPTIVE!</param>
        public KerbalEditException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KerbalEditException" /> class.
        /// </summary>
        /// <param name="message">error message to surface, BE DESCRIPTIVE!</param>
        /// <param name="inner">Underlying exception.</param>
        public KerbalEditException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KerbalEditException" /> class.
        /// </summary>
        /// <param name="info">serialization meta-data <see cref="SerializationInfo" /></param>
        /// <param name="context">serialization stream <see cref="StreamingContext" /></param>
        protected KerbalEditException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
