using System;
using System.Collections.Generic;
using System.IO;

public class CsvData : IEnumerable<CsvRow>
{
    // ---- ---- ---- ---- ---- ---- ---- ----
    // Atributos
    // ---- ---- ---- ---- ---- ---- ---- ----
    private List<CsvRow> csvRowList;
    
    // ---- ---- ---- ---- ---- ---- ---- ----
    // Propiedades
    // ---- ---- ---- ---- ---- ---- ---- ----
    // Datos calculados
    public int Count
    {
        get { return this.csvRowList.Count; }
    }
    
    // Indizadores
    public CsvRow this[int index]
    {
        get { return this.csvRowList[index]; }
    }
    
    // ---- ---- ---- ---- ---- ---- ---- ----
    // Constructores
    // ---- ---- ---- ---- ---- ---- ---- ----
    public CsvData(string textData)
    {
        using (StringReader reader = new StringReader(textData))
        {
            this.csvRowList = new List<CsvRow>();
            
            string line = reader.ReadLine();
            while (line != null)
            {
                this.csvRowList.Add(new CsvRow(line));
                line = reader.ReadLine();
            }
        }
    }
    
    // ---- ---- ---- ---- ---- ---- ---- ----
    // Métodos
    // ---- ---- ---- ---- ---- ---- ---- ----
    // Métodos de "IEnumerable<CsvRow>"
    public IEnumerator<CsvRow> GetEnumerator()
    {
        foreach (CsvRow csvRow in this.csvRowList)
            yield return csvRow;
    }
    
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }
    
}