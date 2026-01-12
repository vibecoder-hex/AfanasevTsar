using System;
using System.Collections.Generic;
using System.Linq;

namespace AVLTree
{
    public class AVLTree
    {
        public class TreeNode // Класс узла
        {
            public int Value { get; set; }
            public TreeNode Left { get; set; }
            public TreeNode Right { get; set; }
            public int Height { get; set; }
            public TreeNode(int value)
            {
                Value = value;
                Left = null;
                Right = null;
                Height = 1;
            }
        }

        public TreeNode Root {  get; set; } // Корень

        private int getHeight(TreeNode node) // Получить высоту
        {
            if (node == null)
                return 0;
            return node.Height;
        }

        private int getBalance(TreeNode node) // Вычислить баланс-фактор
        {
            if (node == null)
                return 0;
            return getHeight(node.Left) - getHeight(node.Right);
        }

        private void updateHeight(TreeNode node) // обновить высоту
        {
            if (node == null)
                return;
            node.Height = 1 + Math.Max(getHeight(node.Left), getHeight(node.Right));
        }

        private TreeNode rightRotate(TreeNode y) // Right поворот
        {
            TreeNode x = y.Left;
            TreeNode T2 = x.Right;

            x.Right = y;
            y.Left = T2;
            updateHeight(y);
            updateHeight(x);
            return x;
        }

        private TreeNode leftRotate(TreeNode x) // Left Поворот
        {
            TreeNode y = x.Right;
            TreeNode T2 = y.Left;

            y.Left = x;
            x.Right = T2;
            updateHeight(x);
            updateHeight(y);
            return y;
        }

        private TreeNode leftRightRotate(TreeNode node) // LR поворот
        {
            node.Left = leftRotate(node.Left);
            return rightRotate(node);
        }

        private TreeNode rightLeftRotate(TreeNode node) // RL поворот
        {
            node.Right = rightRotate(node.Right);
            return leftRotate(node);
        }

        private TreeNode balanceSubtree(TreeNode node) // Балансировка поддера
        {
            updateHeight(node);
            int balanceFactor = getBalance(node);
            if (balanceFactor > 1)
            {
                if (getBalance(node.Left) >= 0)
                {
                    return rightRotate(node);
                }
                else
                {
                    return leftRightRotate(node);
                } 
            }
            else if (balanceFactor < -1)
            {
                if (getBalance(node.Right) <= 0)
                {
                    return leftRotate(node);
                }
                else
                {
                    return rightLeftRotate(node);
                }
            }
            return node;
        }

        private TreeNode insertRecursive(TreeNode node, int value) // Вставка в дерево
        {
            if (node == null)
                return new TreeNode(value);
            else if (value < node.Value)
                node.Left = insertRecursive(node.Left, value);
            else
                node.Right = insertRecursive(node.Right, value);
            return balanceSubtree(node);
        }

        private bool containsRecursive(TreeNode node, int value) // Поиск в дереве
        {
            if (node == null)
                return false;
            else
            {
                if (value == node.Value)
                    return true;
                else if (value < node.Value)
                    return containsRecursive(node.Left, value);
                else 
                    return containsRecursive(node.Right, value);
            }  
        }

        private TreeNode getMinNode(TreeNode node) // Минимальный узел
        {
            TreeNode current = node;
            while (current != null && current.Left != null)
            {
                current = current.Left;
            }
            return current;
        }

        private TreeNode removeRecursive(TreeNode node, int value) // Удаление из узла
        {
            if (node == null)
                return node;
            if (value < node.Value)
                node.Left = removeRecursive(node.Left, value);
            else if (value > node.Value)
                node.Right = removeRecursive(node.Right, value);
            else
            {
                if (node.Left == null)
                    return node.Right;
                if (node.Right == null)
                    return node.Left;
                TreeNode successor = this.getMinNode(node.Right);
                node.Value = successor.Value;
                node.Right = removeRecursive(node.Right, successor.Value);
            }
            return balanceSubtree(node);

        }

        private void inorderTraversalRecursive(TreeNode node, List<int> output) // Обход
        {
            if (node == null) return;
            inorderTraversalRecursive(node.Left, output);
            output.Add(node.Value);
            inorderTraversalRecursive(node.Right, output);
        }

        // Публичные методы
        public void Insert(int value)
        {
            Root = insertRecursive(Root, value);
        }

        public List<int> InorderTraversalData()
        {
            List<int> output = new List<int>();
            inorderTraversalRecursive(Root, output);
            return output;
        }

        public bool Contains(int value)
        {
            return containsRecursive(Root, value);
        }

        public void Remove(int value)
        {
            Root = removeRecursive(Root, value);
        }
    }

    public static class Program
    {
        public static void Main(string[] args)
        {
            AVLTree tree = new AVLTree();
            for (int value = 0; value <= 10; value++)
                tree.Insert(value);
            tree.InorderTraversalData().ForEach(elem => Console.Write($"{elem} "));
        }
    }
}