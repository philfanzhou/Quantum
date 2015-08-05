namespace PF.Domain.FilterTasks.Entities
{
    using Core.Domain;
    using PF.Domain.FilterConditions.Entities;

    public class FilterTask : Entity, IAggregateRoot
    {
        private FilterCondition _filterCondition;

        public string Name { get; set; }

        public FilterTaskStatus Status { get; set; }

        public string ConditionId { get; private set; }

        public virtual FilterCondition Condition
        {
            get { return _filterCondition; }
            set
            {
                _filterCondition = value;
                ConditionId = _filterCondition == null ? null : _filterCondition.Id;
            }
        }

        public virtual FilterResult Result { get; private set; }

        protected FilterTask(string id) : base(id) { }

        protected FilterTask() { }

        public void ApplyCondition()
        {
            if (Condition == null)
            {
                return;
            }

            var conditionresult = Condition.Apply();
            Result = Result ?? new FilterResult(Id);
            Result.UpdateResult(conditionresult);
        }

        public void UpdateResult(FilterResult result)
        {
            Result = Result ?? new FilterResult(Id);
            Result.UpdateResult(result.ConditionResult);
            Result.ExecEndTime = result.ExecEndTime;
            Result.ExecStartTime = result.ExecStartTime;
        }
    }
}
