using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;

namespace ProjCrud
{
    public partial class CompraWindow : Window
    {   

        private Compra _compraSelecionada;
        private ItemPedido _itemSelecionado;
        private bool _modoEdicao = false;
        private List<Compra> compras = new List<Compra>(); // Inicializar para evitar warnings

        public CompraWindow()
        {
            try
            {
                InitializeComponent();
                AtualizarLista();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao inicializar: {ex.Message}");
            }
        }

        private void AtualizarLista()
        {
            try
            {
                compras = ComprasDAO.Ler();
                lstCompras.Items.Clear();
                
                foreach (var compra in compras)
                {
                    lstCompras.Items.Add(compra);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao atualizar lista: {ex.Message}");
            }
        }

        private void Adicionar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validar dados da compra
                if (!string.IsNullOrWhiteSpace(txtCpfCliente.Text) && 
                    int.TryParse(txtIdVendedor.Text, out int idVendedor) &&
                    dpDataCompra.SelectedDate.HasValue) // Verifique se uma data foi selecionada
                {
                    string formaPagamento = "";
                    
                    // Determinar a forma de pagamento
                    if (chkDinheiro.IsChecked == true) formaPagamento = "Dinheiro";
                    else if (chkPix.IsChecked == true) formaPagamento = "PIX";
                    else if (chkCartão.IsChecked == true) formaPagamento = "Cartão";
                    else if (chkBoleto.IsChecked == true) formaPagamento = "Boleto";
                    else formaPagamento = "Não especificada";

                    var novaCompra = new Compra
                    {
                        CpfCliente = txtCpfCliente.Text,
                        IdVendedor = idVendedor,
                        DataCompra = dpDataCompra.SelectedDate.Value.DateTime, // Acessar o DateTime
                        FormaPagamento = formaPagamento,
                        StatusPagamento = "Pendente" // Valor padrão
                    };

                    ComprasDAO.Criar(novaCompra);
                    AtualizarLista();
                    LimparCampos();
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Preencha todos os campos corretamente");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao adicionar compra: {ex.Message}");
            }
        }
        private void Atualizar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lstCompras.SelectedItem is Compra compra &&
                    int.TryParse(txtIdVendedor.Text, out int idVendedor) &&
                    dpDataCompra.SelectedDate.HasValue)
                {
                    string formaPagamento = "";
                    
                    // Determinar a forma de pagamento
                    if (chkDinheiro.IsChecked == true) formaPagamento = "Dinheiro";
                    else if (chkPix.IsChecked == true) formaPagamento = "PIX";
                    else if (chkCartão.IsChecked == true) formaPagamento = "Cartão";
                    else if (chkBoleto.IsChecked == true) formaPagamento = "Boleto";
                    else formaPagamento = "Não especificada";
                    
                    compra.CpfCliente = txtCpfCliente.Text;
                    compra.IdVendedor = idVendedor;
                    compra.DataCompra = dpDataCompra.SelectedDate.Value.DateTime;
                    compra.FormaPagamento = formaPagamento;
                    
                    ComprasDAO.Atualizar(compra);
                    AtualizarLista();
                    LimparCampos();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao atualizar compra: {ex.Message}");
            }
        }

        

        private void LstCompras_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (lstCompras.SelectedItem is Compra compra)
                {
                    _compraSelecionada = compra;
                    
                    txtCpfCliente.Text = compra.CpfCliente;
                    dpDataCompra.SelectedDate = compra.DataCompra;
                    txtIdVendedor.Text = compra.IdVendedor.ToString();
                    
                    // Limpar todos os checkboxes antes
                    chkDinheiro.IsChecked = false;
                    chkPix.IsChecked = false;
                    chkCartão.IsChecked = false;
                    chkBoleto.IsChecked = false;
                    
                    // Selecionar o checkbox correto
                    string formaPagamento = compra.FormaPagamento?.ToLower() ?? "";
                    if (formaPagamento.Contains("dinheiro")) chkDinheiro.IsChecked = true;
                    else if (formaPagamento.Contains("pix")) chkPix.IsChecked = true;
                    else if (formaPagamento.Contains("cartão") || formaPagamento.Contains("cartao")) chkCartão.IsChecked = true;
                    else if (formaPagamento.Contains("boleto")) chkBoleto.IsChecked = true;
                    
                    // Carregar os itens desta compra
                    CarregarItensCompra(compra.Id);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao selecionar compra: {ex.Message}");
            }
        }
        private void LimparCampos()
        {
            txtCpfCliente.Text = string.Empty;
            dpDataCompra.SelectedDate = null; // Limpar a data
            txtIdVendedor.Text = string.Empty;
            chkDinheiro.IsChecked = false;
            chkPix.IsChecked = false;
            chkCartão.IsChecked = false;
            chkBoleto.IsChecked = false;
        }

       private void Pesquisar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string cpfPesquisa = txtPesquisarCpfCliente.Text?.Trim() ?? string.Empty;
                
                if (!string.IsNullOrEmpty(cpfPesquisa))
                {
                    lstCompras.Items.Clear();
                    
                    // Filtrando compras pelo CPF do cliente
                    var comprasEncontradas = ComprasDAO.Ler()
                        .FindAll(c => c.CpfCliente.Contains(cpfPesquisa));
                    
                    if (comprasEncontradas.Count > 0)
                    {
                        foreach (var compra in comprasEncontradas)
                        {
                            lstCompras.Items.Add(compra);
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"Nenhuma compra encontrada para o CPF: {cpfPesquisa}");
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Digite um CPF para pesquisar");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao pesquisar compra: {ex.Message}");
            }
        }

        private void MostrarTodos_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txtPesquisarCpfCliente.Text = string.Empty;
                lstCompras.Items.Clear();
                AtualizarLista();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao mostrar todos: {ex.Message}");
            }
        }

        private void Fechar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CarregarItensCompra(int idCompra)
        {
            try
            {
                lstItens.Items.Clear();
                
                // Aproveita o método ItensPedido do ComprasDAO
                var itens = ComprasDAO.ItensPedido(idCompra);
                
                foreach (var item in itens)
                {
                    // Buscar informações adicionais do livro
                    string tituloLivro = BuscarInformacaoLivro(item.IdLivro, "Titulo");
                    lstItens.Items.Add($"ID: {item.Id} - {tituloLivro} - Qtd: {item.Quantidade} - R$ {item.SubTotal:F2}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao carregar itens: {ex.Message}");
            }
        }

        private string BuscarInformacaoLivro(int idLivro, string campo)
        {
            try
            {
                using (var conexao = Conexao.Conectar())
                {
                    var cmd = new SqlCommand($"SELECT {campo} FROM Livro WHERE Id = @IdLivro", conexao);
                    cmd.Parameters.AddWithValue("@IdLivro", idLivro);
                    var resultado = cmd.ExecuteScalar();
                    
                    if (resultado != null && resultado != DBNull.Value)
                        return resultado.ToString();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao buscar {campo} do livro: {ex.Message}");
            }
            
            return $"Livro #{idLivro}";
        }
        
        // Carregar lista de livros disponíveis
        private void CarregarLivros()
        {
            try
            {
                cmbLivro.Items.Clear();
                
                // Adicionar item inicial como placeholder
                cmbLivro.Items.Add("-- Selecione o Livro --");
                
                using (var conexao = Conexao.Conectar())
                {
                    var cmd = new SqlCommand("SELECT Id, Titulo, Preco FROM Livro WHERE Estoque > 0", conexao);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string titulo = reader.GetString(1);
                            decimal preco = reader.GetDecimal(2);
                            cmbLivro.Items.Add($"{id} - {titulo} - R$ {preco:F2}");
                        }
                    }
                }
                
                // Selecionar o primeiro item
                if (cmbLivro.Items.Count > 0)
                    cmbLivro.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao carregar livros: {ex.Message}");
            }
        }
        
                // Método para adicionar um novo item
        private void AdicionarItem_Click(object sender, RoutedEventArgs e)
        {
            if (_compraSelecionada == null)
            {
                System.Diagnostics.Debug.WriteLine("Selecione uma compra primeiro");
                return;
            }
            
            _modoEdicao = false;
            txtTituloOperacao.Text = "Adicionar Item";
            
            // Limpar campos
            txtIdItem.Text = string.Empty;
            txtQuantidade.Text = "1";
            
            // Carregar livros disponíveis
            CarregarLivros();
            
            // Exibir painel
            pnlItemDetalhes.IsVisible = true;
        }

        // Método para editar quantidade
        private void EditarQuantidade_Click(object sender, RoutedEventArgs e)
        {
            if (_compraSelecionada == null || lstItens.SelectedItem == null)
            {
                System.Diagnostics.Debug.WriteLine("Selecione uma compra e um item");
                return;
            }
            
            try
            {
                // Extrair ID do item do texto selecionado
                string itemText = lstItens.SelectedItem.ToString();
                int idItem = int.Parse(itemText.Split('-')[0].Replace("ID:", "").Trim());
                
                // Buscar o item completo
                var itens = ComprasDAO.ItensPedido(_compraSelecionada.Id);
                _itemSelecionado = itens.FirstOrDefault(i => i.Id == idItem);
                
                if (_itemSelecionado != null)
                {
                    _modoEdicao = true;
                    txtTituloOperacao.Text = "Editar Quantidade";
                    
                    // Preencher campos
                    txtIdItem.Text = _itemSelecionado.Id.ToString();
                    txtQuantidade.Text = _itemSelecionado.Quantidade.ToString();
                    
                    // Selecionar o livro correto
                    for (int i = 0; i < cmbLivro.Items.Count; i++)
                    {
                        string item = cmbLivro.Items[i].ToString();
                        if (item.StartsWith(_itemSelecionado.IdLivro.ToString()))
                        {
                            cmbLivro.SelectedIndex = i;
                            break;
                        }
                    }
                    
                    // Exibir painel
                    pnlItemDetalhes.IsVisible = true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao preparar edição: {ex.Message}");
            }
        }

        // Método para remover item
        private void RemoverItem_Click(object sender, RoutedEventArgs e)
        {
            if (_compraSelecionada == null || lstItens.SelectedItem == null)
            {
                System.Diagnostics.Debug.WriteLine("Selecione uma compra e um item");
                return;
            }
            
            try
            {
                // Extrair ID do item do texto selecionado
                string itemText = lstItens.SelectedItem.ToString();
                int idItem = int.Parse(itemText.Split('-')[0].Replace("ID:", "").Trim());
                
                // Usar o ItemPedidoDAO.Deletar
                ItemPedidoDAO.Deletar(idItem);
                
                // Recalcular o total da compra
                ComprasDAO.CalculoTotal(_compraSelecionada.Id);
                
                // Atualizar a interface
                CarregarItensCompra(_compraSelecionada.Id);
                AtualizarLista();
                
                System.Diagnostics.Debug.WriteLine("Item removido com sucesso");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao remover item: {ex.Message}");
            }
        }

        // Método para confirmar a adição/edição do item
        private void ConfirmarItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbLivro.SelectedItem == null)
                {
                    System.Diagnostics.Debug.WriteLine("Selecione um livro");
                    return;
                }

                if (cmbLivro.SelectedIndex <= 0 || cmbLivro.SelectedItem == null)
                {
                    System.Diagnostics.Debug.WriteLine("Selecione um livro");
                    return;
                }
                
                if (!int.TryParse(txtQuantidade.Text, out int quantidade) || quantidade <= 0)
                {
                    System.Diagnostics.Debug.WriteLine("Quantidade inválida");
                    return;
                }
                
                // Extrair ID do livro do item selecionado no ComboBox
                string livroSelecionado = cmbLivro.SelectedItem.ToString();
                int idLivro = int.Parse(livroSelecionado.Split('-')[0].Trim());
                
                if (_modoEdicao && _itemSelecionado != null)
                {
                    // Usar o método existente para alterar quantidade
                    ItemPedidoDAO.MudarQntPedido(_itemSelecionado.Id, quantidade);
                }
                else
                {
                    // Buscar o maior ID existente para incrementar
                    int novoId = ObterProximoId();
                    
                    // Criar novo item e usar o método existente para adicionar
                    var novoItem = new ItemPedido
                    {
                        Id = novoId,
                        IdCompra = _compraSelecionada.Id,
                        IdLivro = idLivro,
                        Quantidade = quantidade
                    };
                    
                    ItemPedidoDAO.Adicionar(novoItem);
                }
                
                // Recalcular o total da compra
                ComprasDAO.CalculoTotal(_compraSelecionada.Id);
                
                // Atualizar a interface
                CarregarItensCompra(_compraSelecionada.Id);
                AtualizarLista();
                
                // Esconder o painel
                pnlItemDetalhes.IsVisible = false;
                
                System.Diagnostics.Debug.WriteLine("Operação realizada com sucesso");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro: {ex.Message}");
            }
        }

        // Método auxiliar para obter o próximo ID disponível
        private int ObterProximoId()
        {
            try
            {
                using (var conexao = Conexao.Conectar())
                {
                    var cmd = new SqlCommand("SELECT ISNULL(MAX(Id), 0) + 1 FROM ItemPedido", conexao);
                    return (int)cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao obter próximo ID: {ex.Message}");
                return 1; // Valor padrão em caso de erro
            }
        }

        // Método para cancelar a operação
        private void CancelarItem_Click(object sender, RoutedEventArgs e)
        {
            pnlItemDetalhes.IsVisible = false;
        }

        // Método para atualizar as informações do item quando um livro é selecionado
        private void CmbLivro_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbLivro.SelectedItem != null && cmbLivro.SelectedIndex > 0) // Ignora o placeholder
            {
                try
                {
                    // Extrair ID do livro
                    string livroSelecionado = cmbLivro.SelectedItem.ToString();
                    
                    // Verificar se o formato é válido
                    if (livroSelecionado.Contains("-"))
                    {
                        string idPart = livroSelecionado.Split('-')[0].Trim();
                        if (int.TryParse(idPart, out int idLivro))
                        {
                            // Atualizar informações do estoque
                            using (var conexao = Conexao.Conectar())
                            {
                                var cmd = new SqlCommand("SELECT Estoque FROM Livro WHERE Id = @IdLivro", conexao);
                                cmd.Parameters.AddWithValue("@IdLivro", idLivro);
                                var resultado = cmd.ExecuteScalar();
                                
                                if (resultado != null && resultado != DBNull.Value)
                                {
                                    int estoque = (int)resultado;
                                    lblEstoqueDisponivel.Text = $"Estoque disponível: {estoque}";
                                    
                                    // Limitar a quantidade ao estoque disponível
                                    if (int.TryParse(txtQuantidade.Text, out int qntAtual) && qntAtual > estoque)
                                    {
                                        txtQuantidade.Text = estoque.ToString();
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Erro ao atualizar informações do livro: {ex.Message}");
                }
            }
            else
            {
                // Limpar as informações de estoque se nenhum livro válido for selecionado
                lblEstoqueDisponivel.Text = "Estoque disponível: 0";
            }
        }

    }
}
