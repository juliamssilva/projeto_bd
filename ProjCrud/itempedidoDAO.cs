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
                var cmd = conexao.CreateCommand();

                int resultado = livroDAO.AtualizarEstoque(itemPedido.IdLivro, itemPedido.Quantidade);

                if (resultado == -1)
                {
                    throw new Exception("Estoque insuficiente para a quantidade desejada.");
                }
                if (resultado== -2)
                {
                    throw new Exception("Não possui estoque deste livro.");
                }
                if (resultado== -3)
                {
                    throw new Exception("Quantidade inválida.");
                }
                // Comando SQL para inserir um novo item de pedido na tabela ItemPedido
                cmd.CommandText = "INSERT INTO ItemPedido (Id,IdCompra, IdLivro, Quantidade,SubTotal) VALUES (@Id, @IdCompra, @IdLivro, @Quantidade, @SubTotal)";
                cmd.Parameters.AddWithValue("@Id", itemPedido.Id);
                cmd.Parameters.AddWithValue("@IdCompra", itemPedido.IdCompra);
                cmd.Parameters.AddWithValue("@IdLivro", itemPedido.IdLivro);
                cmd.Parameters.AddWithValue("@Quantidade", itemPedido.Quantidade);
                cmd.Parameters.AddWithValue("@SubTotal", itemPedido.SubTotal);
                cmd.ExecuteNonQuery();
            }
        }
        public static void Deletar(int id)
        {
            using (var conexao = Conexao.Conectar())
            {
                var cmd = new SqlCommand("DELETE FROM ItemPedido WHERE id = @id", conexao);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }

        public static void MudarQntPedido(int IdItemPedido, int novaQuantidade)
        {
            using (var conexao = Conexao.Conectar())
            {

                var cmdIdLivro = new SqlCommand("SELECT IdLivro FROM ItemPedido WHERE Id = @Id", conexao);
                cmdIdLivro.Parameters.AddWithValue("@Id", IdItemPedido);
                int IdLivro = (int)cmdIdLivro.ExecuteScalar();

                int resultado = livroDAO.AtualizarEstoque(IdLivro, novaQuantidade);
                if(resultado == -1){
                    throw new Exception("Estoque insuficiente para a quantidade desejada.");
                }
                if(resultado == -2){
                    throw new Exception("Não possui estoque deste livro.");
                }
                if(resultado == -3){
                    throw new Exception("Quantidade inválida.");
                }

                if (novaQuantidade == 0){
                    Deletar(IdItemPedido);
                    return;
                }

                
                var cmd = new SqlCommand("UPDATE ItemPedido SET Quantidade = @novaQuantidade WHERE id = @id", conexao);
                cmd.Parameters.AddWithValue("@id", IdItemPedido);
                cmd.Parameters.AddWithValue("@novaQuantidade", novaQuantidade);
                cmd.ExecuteNonQuery();
                CalcSubtotal(IdLivro);
            }
        }

        public static void CalcSubtotal (int id)
        {
            using (var conexao = Conexao.Conectar())
            {
                var cmdSelectPreco = new SqlCommand("SELECT Preco FROM Livro WHERE Id = @Id", conexao);
                cmdSelectPreco.Parameters.AddWithValue("@Id", id);
                decimal preco = (decimal)cmdSelectPreco.ExecuteScalar();

                var cmdSelectQuantidade = new SqlCommand("SELECT Quantidade FROM ItemPedido WHERE Id = @IdItemPedido", conexao);
                cmdSelectQuantidade.Parameters.AddWithValue("@Id", id);
                int quantidade = (int)cmdSelectQuantidade.ExecuteScalar();

                var cmd = new SqlCommand("UPDATE ItemPedido SET SubTotal = @SubTotal WHERE id = @id", conexao);
                cmd.Parameters.AddWithValue("@id", id);
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
                            Id = reader.GetInt32(0),
                            IdCompra = reader.GetInt32(1),
                            IdLivro = reader.GetInt32(2),
                            Quantidade = reader.GetInt32(3),
                            SubTotal = reader.GetDecimal(4)
                        });
                    }
                }
            }
            return itensPedido;
        }

    }
}

