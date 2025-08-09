namespace TodoApi.Models
{
    using System.ComponentModel;
public enum TodoStatus
{
    [Description("Pendente")]
    Pendente,
    
    [Description("Em Andamento")]
    EmAndamento,
    
    [Description("Concluída")]
    Concluida
}
}