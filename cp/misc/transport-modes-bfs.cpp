#include<bits/stdc++.h>


vector<int> dx {0,0,1,-1};
vector<int> dy {1,-1,0,0};

bool inBound(int p, int q, int r, int c)
{
	return p>=0 && q>=0 && p<r && q<c;
}

int bfs(pair<int,int>& st, pair<int,int>& en, vector<vector<int>>& grid, int mode)
{
	int r = grid.size(), c = grid[0].size();

	queue<pair<int,int>> q;
	q.push(st);
	int dist = 0;

	while(!q.empty())
	{
		int csize = q.size();

		for(int i=0;i<csize;i++)
		{
			auto pos = q.front();
			int px = pos.first, py = pos.second;

			for(int k=0;k<;k++)
			{
				int nx = px + dx[k];
				int ny = py + dy[k];

				if(!inBound(nx,ny,r,c))
					continue;

				if(grid[nx][ny] == -2)
				{
					return dist+1;
				}

				if(grid[nx][ny] < 0)
					continue;

				grid[nx][ny] = -3;
				q.push(make_pair(nx,ny));
			}
		}

		dist++;
	}

	return -1;
}

int findBestTransportMode(vector<vector<char>>& grid, vector<int>& times, vector<int>& costs)
{
	int r = grid.size(), c = grid[0].size();
	int tmodes = times.size();

	pair<int,int> st, en;

	vector<vector<int>> mod_grid(r, vector<int>(c,0));

	for(int i=0;i<r;i++)
	{
		for(int j=0;j<c;j++)
		{
			if(grid[i][j] == 'S')
			{
				mod_grid[i][j] = -1;
				st = make_pair(i,j);
				continue;
			}

			if(grid[i][j] == 'D')
			{
				mod_grid[i][j] = -2;
				en = make_pair(i,j);
				continue;
			}

			if(grid[i][j] == 'X')
			{
				mod_grid[i][j] = -3;
				continue;
			}

			mod_grid[i][j] = grid[i][j] - '0';
		}
	}

	set<int> possible_modes;

	for(int i=0;i<4;i++)
	{
		int nx = st.first + dx[i];
		int ny = st.second + dy[i];

		if(!inBound(nx,ny,r,c))
			continue;

		possible_modes.insert(mod_grid[nx][ny]);
	}

	int min_time = INT_MAX, min_cost = INT_MAX, bestMode = -1;

	for(auto mode : possible_modes)
	{
		int nsteps = bfs(st, en, mod_grid, mode);

		if(nsteps == -1)
			continue;

		int new_min_time = nsteps * times[mode-1];
		int new_min_cost = nsteps * costs[mode-1];

		if(new_min_time < min_time || (new_min_time == min_time && new_min_cost < min_cost))
		{
			min_time = nsteps * times[mode-1];
			min_cost = nsteps * costs[mode-1];
			bestMode = mode;
		}
	}

	return bestMode;
}


signed main()
{
	return 0;
}



