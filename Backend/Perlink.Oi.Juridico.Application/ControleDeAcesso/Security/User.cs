using System;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Application.Security {
    public class User {
        private readonly IList<string> _roles = new List<string>();
        private readonly IList<string> _permissions = new List<string>();

        public Guid Id { get; } = Guid.NewGuid();
        public string Name { get; }
        public string Login { get; }
        public IEnumerable<string> Roles => _roles;
        public IEnumerable<string> Permissions => _permissions;

        protected User() { }

        public User(string name, string login, string password) {
            Name = name;
            Login = login;
        }
    }
}