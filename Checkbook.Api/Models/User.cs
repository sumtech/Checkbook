// Copyright (c) Palouse Coding Conglomerate. All Rights Reserved.

namespace Checkbook.Api.Models
{
    /// <summary>
    /// Represents a user of the application.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or sets the unique identifier for this user.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the user name of this user.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the first name of this user.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the Last name of this user.
        /// </summary>
        public string LastName { get; set; }
    }
}
