namespace PF.Domain.FilterTasks.Entities
{
    using Core.Domain;
    using Core.Infrastructure.Crosscutting;
    using PF.Domain.FilterConditions.Entities;
    using System;

    public class FilterResult : Entity
    {
        private string _serializedConditionResult;

        public DateTime ExecStartTime { get; set; }

        public DateTime ExecEndTime { get; set; }

        public string SerializedConditionResult
        {
            get { return _serializedConditionResult; }
            private set
            {
                _serializedConditionResult = value;
                ConditionResult = ContainerHelper.Resolve<ISerializer>().JsonDeserialize<ConditionResult>(_serializedConditionResult);
            }
        }

        public ConditionResult ConditionResult { get; private set; }

        public FilterResult(string id) : base(id) { }

        protected FilterResult() { }

        public void UpdateResult(ConditionResult result)
        {
            ConditionResult = result;
            _serializedConditionResult = ContainerHelper.Resolve<ISerializer>().JsonSerializer(result);
        }
    }
}