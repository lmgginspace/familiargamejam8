using System;
using System.Collections.Generic;

public class CsvRow : IEnumerable<string>
{
    // ---- ---- ---- ---- ---- ---- ---- ----
    // Atributos
    // ---- ---- ---- ---- ---- ---- ---- ----
    // Datos
    private List<string> itemList;
    
    // Atributos estáticos
    private static readonly char defaultSeparator = ',';
    
    // ---- ---- ---- ---- ---- ---- ---- ----
    // Propiedades
    // ---- ---- ---- ---- ---- ---- ---- ----
    // Datos calculados
    public int Count
    {
        get { return this.itemList.Count; }
    }
    
    // Indizadores
    public string this[int index]
    {
        get { return this.itemList[index]; }
    }
    
    // ---- ---- ---- ---- ---- ---- ---- ----
    // Constructores
    // ---- ---- ---- ---- ---- ---- ---- ----
    public CsvRow(string textRow) : this(textRow, CsvRow.defaultSeparator) { }
    
    public CsvRow(string textData, char separator)
    {
        this.itemList = new List<string>();
        
        string[] textRowPartList = textData.Split(new char[] { separator });
        
        string currentItem = string.Empty;
        bool quotedContent = false;
        foreach (string textRowPart in textRowPartList)
        {
            if (quotedContent)
                currentItem += separator;

            currentItem += textRowPart;

            if (!quotedContent && textRowPart.StartsWith("\""))
            {
                quotedContent = true;
                currentItem = currentItem.Substring(1);
            }
            
            if (quotedContent && textRowPart.EndsWith("\""))
            {
                quotedContent = false;
                currentItem = currentItem.Substring(0, currentItem.Length - 1);
            }
            
            if (!quotedContent)
            {
                this.itemList.Add(currentItem);
                currentItem = string.Empty;
            }
        }
    }
    
    // ---- ---- ---- ---- ---- ---- ---- ----
    // Métodos
    // ---- ---- ---- ---- ---- ---- ---- ----
    // Métodos de "IEnumerable<string>"
    public IEnumerator<string> GetEnumerator()
    {
        foreach (string item in this.itemList)
            yield return item;
    }
    
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }
    
}