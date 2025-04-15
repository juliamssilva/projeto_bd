using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;


namespace ProjCrud
{
    public class ItemPedidoDAO
    {
    public static void Adicionar(ItemPedido itemPedido)
    {
        using (var conexao = Conexao.Conectar())
        {
            // Atualizar o estoque do livro antes de inserir o item
            int resultado = livroDAO.AtualizarEstoque(itemPedido.IdLivro, itemPedido.Quantidade);
            if (resultado == -1 )
            {
                throw new Exception("Erro: Quantidade no estoque insuficiente.");
            }

            if (resultado == -2 )
            {
                throw new Exception("Erro: Quantidade invalida.");
            }

            // Inserir o item no banco de dados
            var cmd = new SqlCommand(@"
                INSERT INTO ItemPedido (IdCompra, IdLivro, Quantidade, SubTotal)
                VALUES (@IdCompra, @IdLivro, @Quantidade, @SubTotal)", conexao);

            cmd.Parameters.AddWithValue("@IdCompra", itemPedido.IdCompra);
            cmd.Parameters.AddWithValue("@IdLivro", itemPedido.IdLivro);
            cmd.Parameters.AddWithValue("@Quantidade", itemPedido.Quantidade);
            cmd.Parameters.AddWithValue("@SubTotal", itemPedido.SubTotal);

            cmd.ExecuteNonQuery();
        }
    }
        public static void Deletar(int id)
        {
            int IdLivro = 0;
            int quantidadeAtual = 0;

            using (var conexao = Conexao.Conectar())
            {   
                var cmd2 = new SqlCommand("SELECT IdLivro, Quantidade FROM ItemPedido WHERE Id = @Id", conexao);
                cmd2.Parameters.AddWithValue("@Id", id);

                using (var reader = cmd2.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        IdLivro = reader.GetInt32(0);
                        quantidadeAtual = reader.GetInt32(1);
                    }
                    else
                    {
                        throw new Exception("Item de pedido não encontrado.");
                    }
                }

                int resultado = livroDAO.AtualizarEstoque(IdLivro, quantidadeAtual * -1);
                
                if(resultado == -1){
                    throw new Exception("Estoque insuficiente para a quantidade desejada.");
                }
                if(resultado == -2){
                    throw new Exception("Não possui estoque deste livro.");
                }

                var cmd = new SqlCommand("DELETE FROM ItemPedido WHERE id = @id", conexao);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }

        public static void MudarQntPedido(int IdItemPedido, int novaQuantidade)
        {
            using (var conexao = Conexao.Conectar())
            {

                var cmdIdLivro = new SqlCommand("SELECT IdLivro, Quantidade FROM ItemPedido WHERE Id = @Id", conexao);
                cmdIdLivro.Parameters.AddWithValue("@Id", IdItemPedido);

                var cmdGetItem = new SqlCommand("SELECT IdLivro, Quantidade FROM ItemPedido WHERE Id = @Id", conexao);
                cmdGetItem.Parameters.AddWithValue("@Id", IdItemPedido);
                
                int IdLivro = 0;
                int quantidadeAtual = 0;
                
                using (var reader = cmdGetItem.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        IdLivro = reader.GetInt32(0);
                        quantidadeAtual = reader.GetInt32(1);
                    }
                    else
                    {
                        throw new Exception("Item de pedido não encontrado.");
                    }
                }

                 // Calcular a diferença de quantidade
                int diferencaQuantidade = novaQuantidade - quantidadeAtual;

                int resultado = livroDAO.AtualizarEstoque(IdLivro, diferencaQuantidade);
                
                if(resultado == -1){
                    throw new Exception("Estoque insuficiente para a quantidade desejada.");
                }
                if(resultado == -2){
                    throw new Exception("Não possui estoque deste livro.");
                }

                if (novaQuantidade == 0){
                    Deletar(IdItemPedido);
                    return;
                }

                
                var cmd = new SqlCommand("UPDATE ItemPedido SET Quantidade = @novaQuantidade WHERE id = @IdItemPedido", conexao);
                cmd.Parameters.AddWithValue("@IdItemPedido", IdItemPedido); 
                cmd.Parameters.AddWithValue("@novaQuantidade", novaQuantidade);
                cmd.ExecuteNonQuery();
                CalcSubtotal(IdItemPedido);
            }
        }

        public static void CalcSubtotal(int idItemPedido)
        {
            using (var conexao = Conexao.Conectar())
            {
                // Primeiro obter o IdLivro do item
                var cmdGetLivro = new SqlCommand("SELECT IdLivro FROM ItemPedido WHERE Id = @Id", conexao);
                cmdGetLivro.Parameters.AddWithValue("@Id", idItemPedido);
                int idLivro = (int)cmdGetLivro.ExecuteScalar();
                
                // Depois buscar o preço usando o IdLivro correto
                var cmdSelectPreco = new SqlCommand("SELECT Preco FROM Livro WHERE Id = @Id", conexao);
                cmdSelectPreco.Parameters.AddWithValue("@Id", idLivro);
                decimal preco = (decimal)cmdSelectPreco.ExecuteScalar();

                // Buscar a quantidade do item
                var cmdSelectQuantidade = new SqlCommand("SELECT Quantidade FROM ItemPedido WHERE Id = @Id", conexao);
                cmdSelectQuantidade.Parameters.AddWithValue("@Id", idItemPedido);
                int quantidade = (int)cmdSelectQuantidade.ExecuteScalar();

                // Atualizar o subtotal
                var cmd = new SqlCommand("UPDATE ItemPedido SET SubTotal = @SubTotal WHERE Id = @Id", conexao);
                cmd.Parameters.AddWithValue("@Id", idItemPedido);
                cmd.Parameters.AddWithValue("@SubTotal", preco * quantidade);
                cmd.ExecuteNonQuery();
            }
        }

        public static List<ItemPedido> Listar()
        {
            var itensPedido = new List<ItemPedido>();

            using (var conexao = Conexao.Conectar())
            {
                var cmd = new SqlCommand("SELECT * FROM ItemPedido", conexao);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        itensPedido.Add(new ItemPedido
                        {
                            Id = (int)reader["Id"],
                            IdCompra = (int)reader["IdCompra"],
                            IdLivro = (int)reader["IdLivro"],
                            Quantidade = (int)reader["Quantidade"],
                            SubTotal = reader["SubTotal"] == DBNull.Value ? 0m : (decimal)reader["SubTotal"]
                        });
                    }
                }
            }
            return itensPedido;
        }

    }
}

