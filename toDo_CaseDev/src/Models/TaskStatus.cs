namespace TaskApi.Models
{
    using System.ComponentModel;

    public enum TaskStatus
    {
        [Description("Pendente")]
        Pendente,

        [Description("Em Andamento")]
        EmAndamento,

        [Description("Concluída")]
        Concluida
    }
}
