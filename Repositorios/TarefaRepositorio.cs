using listaTarefas.Repositirios.Interface;
using SistemaDeTarefas.Models;
using SistemaDeTarefas.Data;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.AspNetCore.Routing.Tree;

namespace listaTarefas;

public class TarefaRepositorio : ITarefaRepositorio
{
    private void usuarioNaoEncontrado(int id){
        throw new Exception($"A tarefa com o ID {id} não foi encontrado");
    }
    private readonly SistemaTarefasDBContext _dbContext;
    public TarefaRepositorio(SistemaTarefasDBContext sistemaTarefasDBContext){
        _dbContext = sistemaTarefasDBContext;
    }
    public async Task<TarefaModel> Adicionar(TarefaModel tarefa)
    {
        await _dbContext.Tarefas.AddAsync(tarefa);
        await _dbContext.SaveChangesAsync();

        return tarefa;
    }

    public async Task<bool> Apagar(int id)
    {
        TarefaModel tarefaPorId = await BuscarPorId(id);
        if(tarefaPorId == null)
            usuarioNaoEncontrado(id);
        _dbContext.Tarefas.Remove(tarefaPorId);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<TarefaModel> Atualizar(TarefaModel tarefa, int id)
    {
        TarefaModel tarefaPorId = await BuscarPorId(id);
        if(tarefaPorId == null)
            usuarioNaoEncontrado(id);
        tarefaPorId.Nome = tarefa.Nome;
        tarefaPorId.Descricao = tarefa.Descricao;
        tarefaPorId.Descricao = tarefa.Descricao;
        tarefaPorId.Status = tarefa.Status;
        tarefaPorId.UsuarioId = tarefa.UsuarioId; 

        _dbContext.Tarefas.Update(tarefaPorId);
        await _dbContext.SaveChangesAsync();

        return tarefaPorId;
    }

    public async Task<TarefaModel> BuscarPorId(int id)
    {
        return await _dbContext.Tarefas
            .Include(x => x.Usuario)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<TarefaModel>> BuscarTodasTarefas()
    {
        return await _dbContext.Tarefas
            .Include(x => x.Usuario)
            .ToListAsync();
    }
}
