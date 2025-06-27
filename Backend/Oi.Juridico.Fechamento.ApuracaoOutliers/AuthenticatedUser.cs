using Shared.Domain.Interface;

namespace Oi.Juridico.Fechamento.ApuracaoOutliers
{
    public class AuthenticatedUser : IAuthenticatedUser
    {

        public AuthenticatedUser()
        {
        }

        public string Login
        {
            get
            {
                return "SISJUR_JOB";
            }
        }
        public string Name
        {
            get
            {
                return "SISJUR_JOB";
            }
        }
    }
}
