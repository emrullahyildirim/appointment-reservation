using Autofac;
using AuthService.Business.Abstract;
using AuthService.Business.Concrete;
using AuthService.DataAccess.Abstract;
using AuthService.DataAccess.Concrete.EntityFramework;

namespace AuthService.Business.DependencyResolvers.Autofac
{
    public class AuthBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Services
            builder.RegisterType<AuthManager>().As<IAuthService>().InstancePerLifetimeScope();
            builder.RegisterType<UserManager>().As<IUserService>().InstancePerLifetimeScope();
            builder.RegisterType<TokenManager>().As<ITokenService>().InstancePerLifetimeScope();
            builder.RegisterType<S2STokenManager>().As<IS2STokenService>().InstancePerLifetimeScope();

            // Data Access
            builder.RegisterType<EfUserDal>().As<IUserDal>().InstancePerLifetimeScope();
            builder.RegisterType<EfOperationClaimDal>().As<IOperationClaimDal>().InstancePerLifetimeScope();
            builder.RegisterType<EfUserOperationClaimDal>().As<IUserOperationClaimDal>().InstancePerLifetimeScope();
            builder.RegisterType<EfEmailVerificationTokenDal>().As<IEmailVerificationTokenDal>().InstancePerLifetimeScope();
            builder.RegisterType<EfPasswordResetTokenDal>().As<IPasswordResetTokenDal>().InstancePerLifetimeScope();
            builder.RegisterType<EfServiceClientDal>().As<IServiceClientDal>().InstancePerLifetimeScope();
        }
    }
}

