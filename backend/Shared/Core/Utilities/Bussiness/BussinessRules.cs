using Core.Utilities.Result;

namespace Core.Utilities.Bussiness
{
    public class BussinessRules
    {
        public static IResult Run(params IResult[] logics)
        {
            foreach (var logic in logics)
            {
                if (!logic.IsSuccess)
                {
                    return logic;
                }
            }
            return null;
        }
    }
}
