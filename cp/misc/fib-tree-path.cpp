#include<bits/stdc++.h>
using namespace std;

string search(int curr, int curr_order, int target, vector<int>& nodes)
{
	if(curr==target)
		return "";

	int lsize = nodes[curr_order-2];

	if(target <= curr + lsize)
		return "L" + search(curr+1, curr_order-2, target, nodes);
	else
		return "R" + search(curr+lsize+1, curr_order-1, target, nodes);

}

string pathInFibonacciTree(int order, int start, int end)
{
	vector<int> nodes(order+1, 0);
	nodes[0] = 1;
	nodes[1] = 1;
	for(int i=2;i<order+1;i++)
	{
		nodes[i] = 1 + nodes[i-2] + nodes[i-1];
	}

	string source = search(0,order,start,nodes);
	string dest = search(0,order,end,nodes);

	int k = 0;
	while(k<source.size() && k<dest.size())
	{
		if(source[k] == dest[k])
			k++;

		break;
	}

	string path = "";
	for(int i=k;i<source.size();i++)
		path += "U";

	return path + dest.substr(k);
}


signed main()
{
	string res = pathInFibonacciTree(5,-1,7);
	cout << res << "\n";
	return 0;
}