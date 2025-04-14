using listaTarefas.Repositirios.Interface;
using SistemaDeTarefas.Models;
using SistemaDeTarefas.Data;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.AspNetCore.Routing.Tree;

namespace listaTarefas;

public class UsuarioRepositorio : IUsuarioRepositorio
{
    private void usuarioNaoEncontrado(int id){
        throw new Exception($"O usuario com o ID {id} não foi encontrado");
    }
    private readonly SistemaTarefasDBContext _dbContext;
    public UsuarioRepositorio(SistemaTarefasDBContext sistemaTarefasDBContext){
        _dbContext = sistemaTarefasDBContext;
    }
    public async Task<UsuarioModel> Adicionar(UsuarioModel usuario)
    {
        await _dbContext.Usuarios.AddAsync(usuario);
        await _dbContext.SaveChangesAsync();

        return usuario;
    }

    public async Task<bool> Apagar(int id)
    {
        UsuarioModel usurioPorId = await BuscarPorId(id);
        if(usurioPorId == null)
            usuarioNaoEncontrado(id);
        _dbContext.Usuarios.Remove(usurioPorId);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<UsuarioModel> Atualizar(UsuarioModel usuario, int id)
    {
        UsuarioModel usuarioPorId = await BuscarPorId(id);
        if(usuarioPorId == null)
            usuarioNaoEncontrado(id);
        usuarioPorId.Nome = usuario.Nome;
        usuarioPorId.Email = usuario.Email;

        _dbContext.Usuarios.Update(usuarioPorId);
        await _dbContext.SaveChangesAsync();

        return usuarioPorId;
    }

    public async Task<UsuarioModel> BuscarPorId(int id)
    {
        return await _dbContext.Usuarios.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<UsuarioModel>> BuscarTodosUsuarios()
    {
        return await _dbContext.Usuarios.ToListAsync();
    }
}
