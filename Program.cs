using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;

namespace leetcode
{
    
    public class ListNode: IComparable {
        public int val;
        public ListNode next;
        public ListNode(int val=0, ListNode next=null) {
            this.val = val;
            this.next = next;
        }

        override public string ToString(){
            string str = "";
            ListNode ptr = this;
            while(ptr != null){
                str+=ptr.val.ToString();
                if(ptr.next != null){
                    str+= "->";
                }
                ptr = ptr.next;
            }
            return str;
        }

        public int CompareTo(object obj) {
            if(obj == null){
                return 0;
            }
            
            // Console.WriteLine($"this.val.CompareTo(node) {this.val.CompareTo(node)}");
            return this.val.CompareTo((obj as ListNode).val);
        }

    }
    
    class Program
    {
        static void Main(string[] args)
        {

            Program Ins = new Program();
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            
            // input  [[1,4,5],[1,3,4],[2,6]]
            var ls = new List<ListNode>();
            for (int i = 0; i < 1000 ; i++)
            {
                ls.Add(new ListNode(1, new ListNode(4, new ListNode(5))));
            }


            // Solution A - Exhaust
            sw.Start();
            var SolutionExhaustResult = Ins.SolutionExhaust(
                ls.ToArray()
                // new ListNode[] {
                //     new ListNode(1, new ListNode(4, new ListNode(5))),
                //     new ListNode(1, new ListNode(3, new ListNode(4))),
                //     new ListNode(2, new ListNode(6)),
                // }
            );
            sw.Stop();

            Console.WriteLine($"solutionA result : {SolutionExhaustResult.ToString()}");
            Console.WriteLine($"solutionA cost : {sw.Elapsed.TotalMilliseconds}ms");


            // Solution B ( Optimize ) -  priority queue implement by min-heap
            sw.Restart();
            var SolutionOptimizedResult = Ins.SolutionOptimized(
                ls.ToArray()
                // new ListNode[] {
                //     new ListNode(1, new ListNode(4, new ListNode(5))),
                //     new ListNode(1, new ListNode(3, new ListNode(4))),
                //     new ListNode(2, new ListNode(6)),
                // }
            );
            sw.Stop();
            
            Console.WriteLine($"solutionB result : {SolutionOptimizedResult.ToString()}");
            Console.WriteLine($"solutionB cost : {sw.Elapsed.TotalMilliseconds}ms");


        }

        // n is the length of elm in linked-list , k is the amount of linked-list
        // exhaust O(n*k) ; eq: 3 * 1000 
        ListNode SolutionExhaust(ListNode[] lists){
            int n = lists.Length; // K 

            if (n == 0){
                return null;
            }

            int idx = 0;
            
            // use a dummy head to link the merged result
            ListNode head = new ListNode(-1);
            // point to the head of result linklist
            ListNode ptr = head;

            while (!lists.All(a=>a==null)) // while inner linkedlists haven't be visited
            {
                int min = int.MaxValue;
                /*
                    {
                        linklist, <- visited every linkedlist
                        linklist,
                        linklist
                    }
                */
                for (int i = 0; i < n; i++)
                {
                    if (lists[i] != null && lists[i].val < min)
                    {
                        min = lists[i].val; // N
                        idx = i;
                    }
                }
                ptr.next = new ListNode(min); // link to the reult
                ptr = ptr.next; // update the pointer
                lists[idx] = lists[idx].next; // replace node which visited by shfiting to next
            }
            return head.next;
        }

        // n is the length of elm in linked-list , k is the amount of linked-list
        // O(n*logk) priority queue implement by min-heap ; eq: 3 * 3
        ListNode SolutionOptimized(ListNode[] lists){
            
            var pq = new PriorityQueue<ListNode>();

            foreach (ListNode node in lists) {
                if(node != null){
                    pq.Enqueue(node);
                }
            }
            
            ListNode head = new ListNode(-1);
            ListNode ptr = head;
                
            while (!pq.isEmpty())
            {
                ListNode minNode = pq.DeQueue(); // log N
                ptr.next = minNode;
                ptr = minNode;

                if (minNode.next != null){
                    pq.Enqueue(minNode.next);
                }
            }

            return head.next;
        }

    }

}
