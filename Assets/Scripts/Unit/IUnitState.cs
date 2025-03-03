public interface IUnitState
{
    void EnterState(UnitCtrl unit);
    void UpdateState(UnitCtrl unit);
    void ExitState(UnitCtrl unit);
}
