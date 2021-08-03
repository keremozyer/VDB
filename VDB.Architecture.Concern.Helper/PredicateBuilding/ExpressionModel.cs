using System.Collections.Generic;

namespace VDB.Architecture.Concern.Helper.PredicateBuilding
{
    public class ExpressionModel
    {
        public string PropertyName { get; set; }
        public ExpressionOperators LogicOperator { get; set; }
        public object Value { get; set; }
        public IEnumerable<ExpressionMemberFunctions> MemberFunctions { get; set; }

        public ExpressionModel(string propertyName, ExpressionOperators logicOperator, object value, params ExpressionMemberFunctions[] memberFunctions)
        {
            PropertyName = propertyName;
            LogicOperator = logicOperator;
            Value = value;
            MemberFunctions = memberFunctions;
        }
    }
}
