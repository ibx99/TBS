using System.Collections.Generic;

public class GridObject
{
    GridPosition gridPosition;
    GridSystem gridSystem;
    List<Unit> unitList;

    public GridObject(GridPosition gridPosition, GridSystem gridSystem)
    {
        this.gridPosition = gridPosition;
        this.gridSystem = gridSystem;
        this.unitList = new List<Unit>();
    }

    public void AddUnit(Unit unit)
    {
        unitList.Add(unit);
    }

    public void RemoveUnit(Unit unit)
    {
        unitList.Remove(unit);
    }

    public List<Unit> GetUnit() => this.unitList;

    public bool HasAnyUnit() => unitList.Count > 0;

    public override string ToString()
    {
        string unitName = string.Empty;
        foreach (Unit unit in unitList)
        {
            unitName += unit.name + "\n";
        }
        return gridPosition.ToString() + "\n" +
            unitName;
    }

}